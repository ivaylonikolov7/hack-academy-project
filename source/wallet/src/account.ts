
import { ec } from '@types/elliptic';
import elliptic from 'elliptic';
import sha256 from 'js-sha256';

class Account {
  public keypair: Object = { privateKey: ec.KeyPair, publicKey: String };

  constructor(privateKey?: string) {
    // console.log('private key', privateKey);
    if (privateKey) {
      this.restore(privateKey);
    } else {
      this.generate();
    }
  }

  restore(privateKey: string) {
    const ec = new elliptic.ec('secp256k1');

    const key = ec.keyFromPrivate(privateKey);

    this.keypair = {
      privateKey,
      publicKey: key.getPublic().encode('hex', false),
    };
  }

  generate() {
    console.log('generate keypair');
  }
 
  sign(message: String) {
    return this.keypair.privateKey.sign(sha256(message));
  }
};


export default Account;