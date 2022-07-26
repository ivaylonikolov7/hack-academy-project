import mongoose from 'mongoose';

const Schema = mongoose.Schema;


// TODO: 
const addressSchema = new Schema({
    address: String,
    time: { 
        type: String,
        required: true
    }
});

export const Address = mongoose.model('Story', addressSchema);
