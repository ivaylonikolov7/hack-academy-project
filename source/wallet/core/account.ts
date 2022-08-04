// @ts-nocheck

import elliptic from 'elliptic';
import sha256 from 'js-sha256';

import { hexToBase64 } from './utils';

const ec = new elliptic.ec('secp256k1');

function hexToBytes(hex) {
  for (var bytes = [], c = 0; c < hex.length; c += 2)
      bytes.push(parseInt(hex.substr(c, 2), 16));
  return bytes;
}

class Account {
  public key: any = {};
  public publicKey: string;
  public privateKey: string;

  constructor(privateKey: string) {
    if (privateKey) {
      this.key = ec.keyFromPrivate(privateKey);
    } else {
      this.key = ec.genKeyPair();
    }

    this.publicKey = this.address();
    this.privateKey = this.key.getPrivate('hex');
  }
 
  sign(message: string) {
    const signature = this.key.sign(sha256(message)).toDER('hex');

    // console.log(this.key.sign(sha256(message)));

    // console.log(message);
    // console.log(message.toString('utf8'));

    // console.log(sha256(message));
    // console.log(sha256(message).toString('utf8'));
    // console.log(hexToBase64(sha256(message)));
    // console.log(hexToBytes(signature));
    // console.log(hexToBytes(hexToBase64(sha256(message))));
    return {
      hash: signature,
      base64: hexToBase64(signature),
    };
  }

  address(): string {
    return this.key.getPublic().encode('hex');
  }
};


export default Account;