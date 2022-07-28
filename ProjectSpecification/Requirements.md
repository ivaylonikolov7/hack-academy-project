---
tags: hack, academy, project
robots: noindex, nofollow
---

# hackAcademy Graduation Project

The academy is almost over. In order to validate the knowledge you gathered durring the lectures and workshops you'll have to work on a graduation project.

There are two options:
- take part in the development of the **[team project](#Team-project)**;
- or work solo on an **[individual](#Individual-project)** project.

# Team Project

## Summary

Develop blockchain network of your own. It should consist of the following software pieces:

- [Node](#Node)
    - produce blocks;
    - store state in db;
    - consensus.
- Miner (could be part of the Node)
    - mine/validate blocks
- [Block explorer](#Block-Explorer)
    - display blocks and transactions
- [Faucet](#Faucet)
    - request funds
- [Wallet](#Wallet)
    - create account
      - read balance
    - create transaction
      - sign
      - send
    - connect to node

Implementation of the above parts of the prooject can be done in tech stack of choice.


## Data structures and type definitions

Hint: start off with a file describing the genesis allocations to be used for generating the fist block when the node is started. E.g.
```json
{
  "genesis_time": "2022-07-20T00:00:00.000000000Z",
  "chain_id": "hack-chain",
  "balances": {
    "xxx": 21000000
  }
}
```

### Account
```typescript
interface Account {
  // Public key of the account in state (hex encoded)
  publicKey: String,
  // Balance of the account
  balance: Number,
  // Nonce number which increments on each
  // transaction that originates from this account
  nonce: Number
}
```

### Block

```typescript
interface Block {
  // index of the block
  index: Number,
  // timestamp
  timestamp: Number,
  // data (array of transactions),
  data: Array<Transaction>,
  // hash of previous block (hex encoded)
  previousBlockHash: String,
  // The magic number that solves the PoW
  nonce: Number,
  // The number of leading zeroes needed to solve PoW
  difficulty: Number,
  // The hash of the current block sha256(JSON.stringify(Block))
  // Encoding: hex
  // Note: Don't include the `currentBlockHash` prop on serialization.
  currentBlockHash: String
}
```

### Transaction

```typescript
interface Transaction {
  // sender's public key (hex encoded)
  sender: String,
  // recepient's public key (hex encoded)
  recipient: String,
  // amount of coins to be transferred
  value: Number,
  // TODO: add specific example of the structure
  // hex encoded signature generated when the sender's private key signs the transaction and confirms the sender has authorized this transaction
  signature: String,
  // Hash of the transaction serialized object sha256(JSON.stringify(Transaction))
  // Encoding: hex
  // Note: Don't include the `hash` prop on serialization
  hash: String,
  // Optional: hex encoded arbitrary size data payload field
  data?: String,
  // Optional: miner's fee
  fee?: Number,
  // Nonce of the tx
  nonce: Number
}
```

## Node

These are sample definitions of the endpoints (if JSON/JSON-RPC is chosen for communication). Modify as needed if chosen otherwise.

🤫 You should figure out how the nodes should communicate between each other and how should they reach consensus around validity of transactions and blocks.

### Mine block
Mine the next block *if the miner is included in the Node software

URL: `/mine`
Method: `POST`
Body: [Block](#Block)
Example:
```json
{
  "index": 0,
  "timestamp": 1658411411435,
  "data": [
    {
      "sender": "9a7586567b7f9e634976dfeb184395783d1385d8162047efdbab0eda348aedf0",
      "recipient": "e3fca228564d6370fcd616ffb7a17a7f37e474d810a0b5e3a5bfa42b6ef28315",
      "value": 1,
      "signature": "WyJlMjBhM2VjMjlkMzM3MGY3OWYiLCAiY2Y5MGFjZDBjMTMyZmZlNTYiXQ=="
    },
    {
      "sender": "9a7586567b7f9e634976dfeb184395783d1385d8162047efdbab0eda348aedf0",
      "recipient": "e3fca228564d6370fcd616ffb7a17a7f37e474d810a0b5e3a5bfa42b6ef28315",
      "value": 1,
      "signature": "WyJlMjBhM2VjMjlkMzM3MGY3OWYiLCAiY2Y5MGFjZDBjMTMyZmZlNTYiXQ=="
    },
    {
      "sender": "9a7586567b7f9e634976dfeb184395783d1385d8162047efdbab0eda348aedf0",
      "recipient": "e3fca228564d6370fcd616ffb7a17a7f37e474d810a0b5e3a5bfa42b6ef28315",
      "value": 1,
      "signature": "WyJlMjBhM2VjMjlkMzM3MGY3OWYiLCAiY2Y5MGFjZDBjMTMyZmZlNTYiXQ=="
    }
  ],
  "previousBlockHash": "5feceb66ffc86f38d952786c6d696c79c2dbc239dd4e91b46729d73a27fb57e9",
  "nonce": 6,
  "difficulty": 0,
  "currentBlockHash": "8f62f3a7dcbcee8c0fb036772e658a6e624d5242adad3a8b985fd1d6f7fe20ef"
}
```
Response: 
  - Success:
    - Code: 200
    - Content:
      ```json
      {
        "message": "OK"
      }
      ```
  - Error:
    - Code: 404

### Pending transactions
Get the transactions that are waiting to be included in a block and mined (in case the Miner is a separate software piece).
URL: `/pending`
Method: `GET`
Response: 
  - Success:
    - Code: 200
    - Content:
      ```json
      {
        "pending": [
          {
            "sender": "9a7586567b7f9e634976dfeb184395783d1385d8162047efdbab0eda348aedf0",
            "recipient": "e3fca228564d6370fcd616ffb7a17a7f37e474d810a0b5e3a5bfa42b6ef28315",
            "value": 1,
            "signature": "WyJlMjBhM2VjMjlkMzM3MGY3OWYiLCAiY2Y5MGFjZDBjMTMyZmZlNTYiXQ=="
          },
          {
            "sender": "9a7586567b7f9e634976dfeb184395783d1385d8162047efdbab0eda348aedf0",
            "recipient": "e3fca228564d6370fcd616ffb7a17a7f37e474d810a0b5e3a5bfa42b6ef28315",
            "value": 1,
            "signature": "WyJlMjBhM2VjMjlkMzM3MGY3OWYiLCAiY2Y5MGFjZDBjMTMyZmZlNTYiXQ=="
          },
          {
            "sender": "9a7586567b7f9e634976dfeb184395783d1385d8162047efdbab0eda348aedf0",
            "recipient": "e3fca228564d6370fcd616ffb7a17a7f37e474d810a0b5e3a5bfa42b6ef28315",
            "value": 1,
            "signature": "WyJlMjBhM2VjMjlkMzM3MGY3OWYiLCAiY2Y5MGFjZDBjMTMyZmZlNTYiXQ=="
          }
        ]
      }
      ```
  - Error:
    - Code: 404

### New transaction
Send a transaction

URL: `/send`
Method: `POST`
Body: [Transaction](#Transaction)
Example:
```json
{
  "sender": "9a7586567b7f9e634976dfeb184395783d1385d8162047efdbab0eda348aedf0",
  "recipient": "e3fca228564d6370fcd616ffb7a17a7f37e474d810a0b5e3a5bfa42b6ef28315",
  "value": 1,
  "signature": "WyJlMjBhM2VjMjlkMzM3MGY3OWYiLCAiY2Y5MGFjZDBjMTMyZmZlNTYiXQ=="
}
```
Response: 
  - Success:
    - Code: 200
    - Content:
      ```json
      {
        "message": "OK"
      }
      ```
  - Error:
    - Code: 404

### Block info
Get block information by index

URL: `/block/{index}`
Method: `GET`
Response: 
  - Success:
    - Code: 200
    - Content: [Block](#Block)
      - Example
        ```json
        {
          "index": 0,
          "timestamp": 1658411411435,
          "data": [
            {
              "sender": "9a7586567b7f9e634976dfeb184395783d1385d8162047efdbab0eda348aedf0",
              "recipient": "e3fca228564d6370fcd616ffb7a17a7f37e474d810a0b5e3a5bfa42b6ef28315",
              "value": 1,
              "signature": "WyJlMjBhM2VjMjlkMzM3MGY3OWYiLCAiY2Y5MGFjZDBjMTMyZmZlNTYiXQ=="
            },
            {
              "sender": "9a7586567b7f9e634976dfeb184395783d1385d8162047efdbab0eda348aedf0",
              "recipient": "e3fca228564d6370fcd616ffb7a17a7f37e474d810a0b5e3a5bfa42b6ef28315",
              "value": 1,
              "signature": "WyJlMjBhM2VjMjlkMzM3MGY3OWYiLCAiY2Y5MGFjZDBjMTMyZmZlNTYiXQ=="
            },
            {
              "sender": "9a7586567b7f9e634976dfeb184395783d1385d8162047efdbab0eda348aedf0",
              "recipient": "e3fca228564d6370fcd616ffb7a17a7f37e474d810a0b5e3a5bfa42b6ef28315",
              "value": 1,
              "signature": "WyJlMjBhM2VjMjlkMzM3MGY3OWYiLCAiY2Y5MGFjZDBjMTMyZmZlNTYiXQ=="
            }
          ],
          "previousBlockHash": "5feceb66ffc86f38d952786c6d696c79c2dbc239dd4e91b46729d73a27fb57e9",
          "nonce": 6,
          "difficulty": 0,
          "currentBlockHash": "8f62f3a7dcbcee8c0fb036772e658a6e624d5242adad3a8b985fd1d6f7fe20ef"
        }
        ```
  - Error:
    - Code: 404

### Transaction info
Get transaction information by hash

URL: `/transaction/{hash}`
Method: `GET`
Response: 
  - Success:
    - Code: 200
    - Content: [Transaction](#Transaction)
      - Example:
        ```json
        {
          "sender": "9a7586567b7f9e634976dfeb184395783d1385d8162047efdbab0eda348aedf0",
          "recipient": "e3fca228564d6370fcd616ffb7a17a7f37e474d810a0b5e3a5bfa42b6ef28315",
          "value": 1,
          "signature": "WyJlMjBhM2VjMjlkMzM3MGY3OWYiLCAiY2Y5MGFjZDBjMTMyZmZlNTYiXQ==",
          "hash": "45ce75611006ea02b3cf6394f78c0fdd5953a9e86d82c757f23cd4f41c7d601b"
        }
        ```
  - Error:
    - Code: 404

### Account info
Get account info by pubkey

URL: `/account/{pubkey}`
Method: `GET`
Response:
  - Success:
    - Code: 200
    - Content: [Account](#Account)
      - Example
        ```json
        {
          "publicKey": "9a7586567b7f9e634976dfeb184395783d1385d8162047efdbab0eda348aedf0",
          "balance": 3,
          "nonce": 2
        }
        ```
  - Error:
    - Code: 404

## Wallet

The wallet could be a simple CLI project or a standalone desktop/browser based if you chose so.

Example of CLI based functinoality:

```shell
## Create keypair
hackWallet create <MY_ACCOUNT_ALIAS>

## Optional: import mnemonic/privatekey
## Use bip39 (HD wallet) if adding support for mnemonic import.
hackWallet import <MY_ACCOUNT_ALIAS> --mnemonic=...
hackWallet import <MY_ACCOUNT_ALIAS> --privatekey=...

## Connect to node
hackWallet config node <URL>

## Read balance
hackWallet balance <MY_ACCOUNT_ALIAS>
2

## Create transaction
## -b|--broadcast - optional flag for broadcasting
## -s|--sign - optional flag for signing
## -o|--output - optional flag to output to file in case its not directly broadcasted
## -i|--input - path to local transaction file.
hackWallet send -i transaction.json -b
hackWallet send <MY_ACCOUNT_ALIAS> <AMOUNT> <RECIPIENT_PUBKEY> -b
hackWallet send <MY_ACCOUNT_ALIAS> <AMOUNT> <RECIPIENT_PUBKEY> -s
hackWallet send <MY_ACCOUNT_ALIAS> <AMOUNT> <RECIPIENT_PUBKEY> -s -b
hackWallet send <MY_ACCOUNT_ALIAS> <AMOUNT> <RECIPIENT_PUBKEY> -o transaction.json
hackWallet send <MY_ACCOUNT_ALIAS> <AMOUNT> <RECIPIENT_PUBKEY> -s -o transaction.json
```

## Block Explorer

This could be a simple frontend app that consumes the node API and allows user to query:
  - blocks;
  - transactions;
  - accounts.

## Faucet

Simple frontend app that can send you funds on requrest.

Should provide simple form/input for the user to enter their public key and prepare, sign and broadcast a transaction to transfer pre-defined amount of coins to them.

---

## Project defense

- The project code should be available in a Github repo for assesment.
- The project will have to be presented and showcased on the last meeting for final assesment and academy completion.

---
# Individual project

## Summary

Develop a decentralized application consisted of a smart contract and UI with one of the following functionalities:

- ...
- ...

Implementation could be done on a chain and tech/stack of choice.

## Requirements

- Smart contracts;
- Unit tests;
- Deploy scripts;
  - Testnet deployment for showcasing;
- Frontend for contract interaction;
  - Wallet connect.

---

## Project defense

- The project code should be available in a Github repo for assesment.
- The project will have to be presented and showcased on the last meeting for final assesment and academy completion.



