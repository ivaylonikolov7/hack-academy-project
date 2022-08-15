import React, { useState } from 'react';
import './css/App.css';
import axios from 'axios';

function App() {
  const [address, setAddress] = useState('');
  const [hash, setHash] = useState('');

  const [addressEntered, setAddressEntered] = useState(false);

  const send = () => {
    if (!isAddress) {
      return;
    }
    axios.post(`/send`, {
      address: address,
    })
    .then(function (response) {
      console.log(response)
      console.log("Data:")
      console.log(response.data)
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
      <input
        type="text"
        id="address"
        placeholder='enter address'
        onChange={(event) => setAddress(event.target.value)}
        onKeyUp={() => setAddressEntered(true)}
        onPaste={() => setAddressEntered(true)}
      />
      {addressEntered && !isAddress && <p id="error-address">Invalid address</p>}
      <button
        id="send"
        onClick={send}
        disabled={!isAddress}>
          Send
      </button>
      {hash && <div>
        <span>Transaction URL: </span>
        <a href={`http://hackchain.pirin.pro/api/transactions/${hash}`}>http://hackchain.pirin.pro/api/transactions/{hash}</a>
      </div>}
    </div>
  );
}

export default App;
