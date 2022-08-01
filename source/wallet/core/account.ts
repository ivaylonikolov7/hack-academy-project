// @ts-nocheck

import elliptic from 'elliptic';
import sha256 from 'js-sha256';

const ec = new elliptic.ec('secp256k1');

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
    return this.key.sign(sha256(message)).toDER('hex');
  }

  address(): string {
    return this.key.getPublic().encode('hex');
  }
};


export default Account;