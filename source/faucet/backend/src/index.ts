import express, { Express, NextFunction, Request, Response } from 'express';
import dotenv from 'dotenv';
import mongoose from 'mongoose';
import { Address } from './models/address-schema'

dotenv.config();

const app: Express = express();
const port = process.env.PORT;

//Set up default mongoose connection
const mongoDB = 'mongodb://127.0.0.1/faucet';
mongoose.connect(mongoDB);
const db = mongoose.connection;
db.on('error', console.error.bind(console, 'MongoDB connection error:'));

app.post('/send', async (req: Request, res: Response, next: NextFunction) => {
  var date = new Date(); 
  date.setDate(date.getDate() - 1);
  let user = await Address.findOne({ address: req.params.address });
  if(user && new Date(user.time) < date) {
    return next('Too greedy');
  }
  else if(user) {
    user.time = Date.now.toString();
  }
  else {
    user = new Address ({
      addres: req.params.address,
      time: Date.now.toString()
    })
  }
  
  user.save();
  console.log(`TODO: call wallet here with ${req.params.address} address`);
  // TODO: the wallet to return transaction hash to show
  res.send('Return transaction hash here');
});

app.listen(port, () => {
  console.log(`⚡️[server]: Server is running at https://localhost:${port}`);
});
