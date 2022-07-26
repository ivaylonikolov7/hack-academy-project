import React, { useState } from 'react';
import './css/App.css';

function App() {
  const [address, setAddress] = useState('');

  const send = () => {
    //validations and sanity checks
    // TODO: send post request with address
    console.log(address);
  }

  return (
    <div className="App">
      <input type="text" id="address" placeholder='enter address' onChange={(event) => setAddress(event.target.value)}></input>
      <button id="send" onClick={send}>Send</button>
    </div>
  );
}

export default App;
