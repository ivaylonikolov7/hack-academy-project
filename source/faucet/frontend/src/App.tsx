import React, { useState } from 'react';
import './css/App.css';
import axios from 'axios';

function App() {
  const [address, setAddress] = useState('');
  const [hash, setHash] = useState('');
  const [hasError, setHasError] = useState('');

  const [addressEntered, setAddressEntered] = useState(false);

  const send = () => {
    setHash('');
    setHasError('');
    if (!isAddress) {
      return;
    }
    axios.post(`/send`, {
      address: address,
    })
    .then(function (response) {
      // console.log(response)
      // console.log("Data:")
      // console.log(response.data)
      setHash(response.data);
    })
    .catch(function (error) {
      setHasError(error);
      console.log(error);
    });
    console.log(address);
  }

  const isAddress = /^[0-9a-fA-F]{130}/.test(address);

  return (
    <div className="App">
      <h1>HACK ACADEMY FAUCET</h1>
      <h2>Fast and reliable. 1 TestHCT HCT/day</h2>
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
        <a href={`http://hackchain.pirin.pro/api/transactions/${hash}`} target="_blank" rel="noreferrer">http://hackchain.pirin.pro/api/transactions/{hash}</a>
      </div>}
      {hasError && <p>{hasError}</p>}
    </div>
  );
}

export default App;
