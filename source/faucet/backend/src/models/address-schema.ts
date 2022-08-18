import mongoose from 'mongoose';

const Schema = mongoose.Schema;

const addressSchema = new Schema({
  address: { type: String, required: true },
  time: { type: String, required: true }
});

export const Address = mongoose.model('Story', addressSchema);
