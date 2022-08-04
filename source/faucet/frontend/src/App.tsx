import React, { useState } from 'react';
import './css/App.css';
import axios from 'axios';

function App() {
  const [address, setAddress] = useState('');
  const [hash, setHash] = useState('');

  const send = () => {
    if (!isAddress) {
      return;
    }
    axios.post(`${process.env.BACKEND_URL}/send`, {
      address: address,
    })
    .then(function (response) {
      setHash(response.data);
    })
    .catch(function (error) {
      // TODO show error
      console.log(error);
    });
    console.log(address);
  }

  const isAddress = /^[0-9a-fA-F]{130}/.test(address);

  return (
    <div className="App">
      <h1>HACK ACKADEMY FAUCET</h1>
      <h2>Fast and reliable. 0.5 TestHCT HCT/day</h2>
      <input type="text" id="address" placeholder='enter address' onChange={(event) => setAddress(event.target.value)}></input>
      {!isAddress && <text id="error-address">Invalid address</text>}
      <button id="send" onClick={send}>Send</button>
      {hash && <p>Transaction URL: https://TODO_EXPLORER_URL/transaction/{hash}</p>}
    </div>
  );
}

export default App;
