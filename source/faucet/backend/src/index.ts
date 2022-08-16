/* eslint-disable */
// @ts-nocheck
import express, { Express, NextFunction, Request, Response } from 'express';
import dotenv from 'dotenv';
import mongoose from 'mongoose';
import { Address } from './models/address-schema';
import cors from 'cors';
import { Account, Node, Wallet } from hackchain-wallet-core;

dotenv.config();

if (typeof btoa === 'undefined') {
  global.btoa = function (str) {
    return Buffer.from(str, 'binary').toString('base64');
  };
}

const app: Express = express();
app.use(express.urlencoded({ extended: true }));
app.use(express.json());

const port = process.env.PORT;
const privateKey = process.env.PRIVATEKEY || '74186b712dea603adfb68166b18289f81566c9022ea1127b92fcb67b56193f9f';
const chainUrl = process.env.CHAIN_URL || 'http://hackchain.pirin.pro';
const chainId = process.env.CHAIN_ID || 'mainnet';
const amount = process.env.AMOUNT || 1;
const fee = process.env.AMOUNT || 1;

// BEGIN: for debug
// const account = new Account(''); // (privateKey);
// console.log('Keys:');
// console.log(account.key.getPrivate('hex'));
// console.log(account.key.getPublic().encode('hex'));
// END
const account = new Account(privateKey);
const nodes = [];
const node = new Node(chainId, chainUrl);
nodes.push(node);

const wallet = new Wallet({
  nodes,
});

wallet.selectNode(chainId);
wallet.addAccount(account, { selected: true });

//Set up default mongoose connection
const mongoDB = 'mongodb://127.0.0.1/faucet';
mongoose.connect(mongoDB);
const db = mongoose.connection;
db.on('error', console.error.bind(console, 'MongoDB connection error:'));

app.use(cors({ origin: true }));

app.post('/send', async (req: Request, res: Response, next: NextFunction) => {
  /* For demo purposes the following function is in a comment.
  For real situation should be uncommented and one transaction could be made from one ip once per day */
  // const date = new Date(); 
  // date.setDate(date.getDate() - 1);
  // let user = await Address.findOne({ address: req.body.address });
  // if(user && new Date(user.time) < date) {
  //   return next('Too greedy');
  // }
  // else if(user) {
  //   user.time = Date.now.toString();
  // }
  // else {
  //   user = new Address ({
  //     addres: req.body.address,
  //     time: Date.now.toString()
  //   })
  // }
  // user.save();

  const result: any = await wallet.sendTransaction({
    recipient: req.body.address,
    value: Number(amount),
    fee: Number(fee),
  });
  res.send(result.hash);
});

app.listen(port, () => {
  console.log(`⚡️[server]: Server is running at https://localhost:${port}`);
});

export default app;
