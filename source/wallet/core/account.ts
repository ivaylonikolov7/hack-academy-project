// @ts-nocheck

import elliptic from 'elliptic';
import sha256 from 'js-sha256';

const ec = new elliptic.ec('secp256k1');

class Account {
  public key: any = {};

  constructor(privateKey: string) {

    if (privateKey) {
      this.key = ec.keyFromPrivate(privateKey);
    } else {
      this.key = ec.genKeyPair();
    }

    // const key = ec.keyFromPrivate(privateKey);
    // this.keypair = keypair;

    // console.log('private key', privateKey);
    // if (privateKey) {
    //   this.restore(alias, privateKey);
    // } else {
    //   this.generate(alias);
    // }
  }

  // restore(alias: string, privateKey: string) {
  //   

  //   const key = ec.keyFromPrivate(privateKey);

  //   this.keypair = {
  //     privateKey: key,
  //     publicKey: key.getPublic().encode('hex', false),
  //   };


  // }

  // generate(alias: string) {
  //   console.log('generate keypair');
  // }
 
  sign(message: string) {
    return this.key.sign(sha256(message)).toDER('hex');
  }

  address(): string {
    return this.key.getPublic().encode('hex');
  }

  signTransaction() {
    
  }
};


export default Account;