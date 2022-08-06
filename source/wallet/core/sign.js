// @ts-nocheck

const elliptic = require('elliptic');
const sha256 = require('js-sha256');
hexToBase64 = (hexStr)=> {
    return btoa([...hexStr].reduce((acc, _, i) =>
        acc += !(i - 1 & 1) ? String.fromCharCode(parseInt(hexStr.substring(i - 1, i + 1), 16)) : "" 
    ,""));
}


// Buffer.from(str, 'base64') Buffer.toString('base64')


const targetSignature = "MEUCIQDEeuXemxc0M0t8jCGe6BJp9S62SJHE5+BmiGaPZ8ygjgIgP+L6o5YkdhXm32U2ZROb1Fg46i2U6UEH7doT12sjE58=";

const ec = new elliptic.ec('secp256k1');

const message = '{"Sender":"04140188cbfa31e9364dcfe0b6204b4ff7913daf66da8cf59eaf4694ed5d3774e424b20a1a95a829976778ac280496dc4cdd1d1fdfab649cba799d084dcf0cc296","Recipient":"044842ce6522e4442ccf446c9d28e7be0aa26b83934d60289e3c1f9eba49e44dd6134b7b22ff8a38e86e69683843e3f058f326001ff4fee56e94a1e9681cda4bda","Nonce":1,"Value":1000,"Fee":5}';
const key = ec.keyFromPrivate("83f919649688da47e81ea3802d49b902d0367b027ca708dbf7a078b844f196b5");
const publicKey = key.getPublic("hex");
const parsedPublicKey = ec.keyFromPublic(publicKey, 'hex')
console.log("public key: " + publicKey) ;

const hash = sha256(message);
const signature = key.sign(hash);

const signatureHex = signature.toDER('hex');
const signatureHexBase64 = Buffer.toString(signatureHex, 'base64');


console.log(`Message:  '${message}'`);
console.log(`sha256(message):  '${hash}'`);
console.log(`signature.r:  '${signature.r}'`);
console.log(`signature.s:  '${signature.s}'`);
console.log(`signature.toDER('hex'):  '${signatureHex}'`);
console.log(`hexToBase64(signatureHex):  '${signatureHexBase64}'`);
// console.log(this.key.sign(sha256(message)));

// console.log(message);
// console.log(message.toString('utf8'));

// console.log(sha256(message));
// console.log(sha256(message).toString('utf8'));
// console.log(hexToBase64(sha256(message)));
// console.log(hexToBytes(signature));
// console.log(hexToBytes(hexToBase64(sha256(message))));
console.log("signatures match: " + targetSignature === signatureHexBase64);
console.log("signature valid: " + parsedPublicKey.verify(hash, signature))