// Minimal raw protobuf wire-format decoder (no .proto schema needed) - mimics `protoc --decode_raw`.
// Built to reverse-engineer ACE's .carsetup file format (Documents\ACE or Saved Games\ACE\Car
// Setups\<car>\<track>\*.carsetup) - see AceSetupSchema.cs and AceCarInfoProvider.cs's SetupSchema
// data, all derived using this script car-by-car. No .proto file exists for this format; every
// field number's meaning was worked out purely from diffing before/after saves against a known
// UI change (e.g. "changed Front ARB from 2 clicks to 3, which raw field moved?").
//
// Usage (requires Node.js, no dependencies):
//   node decode-ace-setup.js <file.carsetup>              - print full field tree
//   node decode-ace-setup.js <default.carsetup> <changed.carsetup>
//                                                          - print only the fields that differ
// The diff mode is the actual workhorse - save a Default Setup, make one isolated change in-game,
// save again under a new name, then diff the two to find which raw field number moved.
//
// Known gotchas discovered using this on real ACE files (see AceSetupSchema.cs's own comments for
// the full field map):
//   - Some fields drift by the same fixed delta on every save regardless of what changed in-game -
//     a one-time "session settle" artifact, not real signal. If a field's delta is identical across
//     multiple unrelated tests in the same game session, treat it as noise.
//   - A field being completely absent (vs present-but-unchanged-from-default) matters: proto3 omits
//     zero/default values entirely, so "absent" can mean either "at its default" or "this car's
//     physics model has no data here at all" (e.g. no differential, no rear wing) - only
//     distinguishable by checking whether the UI actually offers a control for it.
//   - Compound/enum-style fields (tyre compound, etc.) are typically a per-car LIST INDEX, not a
//     shared global enum - index 0 is always that car's default compound (hence omitted), so the
//     same raw value means a different compound name on different cars.
//
// Wire types: 0=varint, 1=fixed64, 2=length-delimited, 5=fixed32.
const fs = require('fs');

function readVarint(buf, offset) {
  let result = 0n;
  let shift = 0n;
  let pos = offset;
  while (true) {
    if (pos >= buf.length) throw new Error('truncated varint');
    const byte = buf[pos];
    pos++;
    result |= BigInt(byte & 0x7f) << shift;
    if ((byte & 0x80) === 0) break;
    shift += 7n;
    if (shift > 70n) throw new Error('varint too long');
  }
  return [result, pos];
}

function isPrintableAscii(buf) {
  if (buf.length === 0) return false;
  for (const b of buf) {
    if (b < 9 || (b > 13 && b < 32) || b > 126) return false;
  }
  return true;
}

// Try to parse buf as a sequence of valid protobuf fields. Returns the parsed field list,
// or null if anything about it looks invalid (field number 0, bad wire type, truncation) -
// callers use null to fall back to showing raw bytes instead of a bogus nested message.
function tryParseFields(buf) {
  const fields = [];
  let pos = 0;
  try {
    while (pos < buf.length) {
      const [tag, afterTag] = readVarint(buf, pos);
      const fieldNumber = tag >> 3n;
      const wireType = Number(tag & 7n);
      if (fieldNumber === 0n || wireType === 3 || wireType === 4 || wireType === 6 || wireType === 7) {
        return null;
      }
      pos = afterTag;

      if (wireType === 0) {
        const [value, next] = readVarint(buf, pos);
        fields.push({ fieldNumber, wireType, value });
        pos = next;
      } else if (wireType === 1) {
        if (pos + 8 > buf.length) return null;
        fields.push({ fieldNumber, wireType, double: buf.readDoubleLE(pos), raw: buf.readBigUInt64LE(pos) });
        pos += 8;
      } else if (wireType === 2) {
        const [len, afterLen] = readVarint(buf, pos);
        const length = Number(len);
        if (afterLen + length > buf.length) return null;
        fields.push({ fieldNumber, wireType, bytes: buf.subarray(afterLen, afterLen + length) });
        pos = afterLen + length;
      } else if (wireType === 5) {
        if (pos + 4 > buf.length) return null;
        fields.push({ fieldNumber, wireType, float: buf.readFloatLE(pos), raw: buf.readUInt32LE(pos) });
        pos += 4;
      }
    }
  } catch {
    return null;
  }
  return fields;
}

// Builds a tree of {fieldNumber, kind, value, children} nodes from raw bytes.
function buildTree(buf) {
  const fields = tryParseFields(buf);
  if (fields === null) {
    if (buf.length % 4 === 0 && buf.length > 0) {
      const floats = [];
      for (let i = 0; i < buf.length; i += 4) floats.push(buf.readFloatLE(i));
      return [{ fieldNumber: null, kind: 'rawFloats', value: floats }];
    }
    return [{ fieldNumber: null, kind: 'rawHex', value: buf.toString('hex') }];
  }

  return fields.map((f) => {
    if (f.wireType === 0) {
      return { fieldNumber: f.fieldNumber, kind: 'varint', value: f.value };
    } else if (f.wireType === 1) {
      return { fieldNumber: f.fieldNumber, kind: 'fixed64', value: f.double };
    } else if (f.wireType === 5) {
      return { fieldNumber: f.fieldNumber, kind: 'fixed32', value: f.float };
    } else if (f.wireType === 2) {
      if (isPrintableAscii(f.bytes)) {
        return { fieldNumber: f.fieldNumber, kind: 'string', value: f.bytes.toString('latin1') };
      }
      return { fieldNumber: f.fieldNumber, kind: 'message', children: buildTree(f.bytes) };
    }
  });
}

function printTree(nodes, indent, path) {
  const pad = '  '.repeat(indent);
  // Track repetition index per field number at this level, so repeated siblings (e.g. per-corner
  // blocks) get a distinguishable path like $.2[0], $.2[1] instead of colliding on $.2.
  const seenCount = new Map();
  for (const node of nodes) {
    const key = node.fieldNumber === null ? '' : String(node.fieldNumber);
    const count = seenCount.get(key) || 0;
    seenCount.set(key, count + 1);
    const isRepeated = nodes.filter((n) => n.fieldNumber === node.fieldNumber).length > 1;
    const label = node.fieldNumber === null ? path : `${path}.${node.fieldNumber}${isRepeated ? `[${count}]` : ''}`;

    if (node.kind === 'message') {
      console.log(`${pad}${label} (message, ${node.children.length} field(s)):`);
      printTree(node.children, indent + 1, label);
    } else if (node.kind === 'rawFloats') {
      console.log(`${pad}${label} raw floats: [${node.value.map((v) => v.toPrecision(6)).join(', ')}]`);
    } else if (node.kind === 'rawHex') {
      console.log(`${pad}${label} raw hex: ${node.value}`);
    } else {
      console.log(`${pad}${label} (${node.kind}): ${node.value}`);
    }
  }
}

// Flattens a tree into path -> leaf-value entries, using the same [index] disambiguation as printTree.
function flatten(nodes, path, out) {
  for (const node of nodes) {
    const isRepeated = nodes.filter((n) => n.fieldNumber === node.fieldNumber).length > 1;
    const count = flatten.counters.get(`${path}.${node.fieldNumber}`) || 0;
    flatten.counters.set(`${path}.${node.fieldNumber}`, count + 1);
    const label = node.fieldNumber === null ? path : `${path}.${node.fieldNumber}${isRepeated ? `[${count}]` : ''}`;

    if (node.kind === 'message') {
      flatten(node.children, label, out);
    } else if (node.kind === 'rawFloats') {
      out.set(label, `floats:[${node.value.map((v) => v.toPrecision(6)).join(', ')}]`);
    } else if (node.kind === 'rawHex') {
      out.set(label, `hex:${node.value}`);
    } else {
      out.set(label, String(node.value));
    }
  }
  return out;
}
flatten.counters = new Map();

function diffFiles(pathA, pathB) {
  const treeA = buildTree(fs.readFileSync(pathA));
  flatten.counters = new Map();
  const flatA = flatten(treeA, '$', new Map());

  const treeB = buildTree(fs.readFileSync(pathB));
  flatten.counters = new Map();
  const flatB = flatten(treeB, '$', new Map());

  const allKeys = new Set([...flatA.keys(), ...flatB.keys()]);
  const sortedKeys = [...allKeys].sort();
  let anyDiff = false;
  for (const key of sortedKeys) {
    const a = flatA.has(key) ? flatA.get(key) : '<absent>';
    const b = flatB.has(key) ? flatB.get(key) : '<absent>';
    if (a !== b) {
      anyDiff = true;
      console.log(`${key}: ${a}  ->  ${b}`);
    }
  }
  if (!anyDiff) console.log('No differences found.');
}

const [fileA, fileB] = process.argv.slice(2);
if (fileB) {
  console.log(`Diffing:\n  A: ${fileA}\n  B: ${fileB}\n`);
  diffFiles(fileA, fileB);
} else {
  const buf = fs.readFileSync(fileA);
  console.log(`File size: ${buf.length} bytes\n`);
  printTree(buildTree(buf), 0, '$');
}
