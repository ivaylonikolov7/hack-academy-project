import React, { useState } from 'react';
import './css/App.css';
import axios from 'axios';

function App() {
  const [address, setAddress] = useState('');
  const [hash, setHash] = useState('');

  const send = () => {
    // TODO validations and sanity checks
    axios.post('/send', {
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

  return (
    <div className="App">
      <input type="text" id="address" placeholder='enter address' onChange={(event) => setAddress(event.target.value)}></input>
      <button id="send" onClick={send}>Send</button>
      {hash && <p>Transaction URL: https://TODO_EXPLORER_URL/transaction/{hash}</p>}
    </div>
  );
}

export default App;
