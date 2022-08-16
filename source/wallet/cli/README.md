# Wallet CLI
## Project Setup

```sh
npm install
```

### Compile and Minify for Production

```sh
npm run build
```

### Usage
Import private key
```sh
node dist/index.js import account1 --privateKey="key"
```

Create public/private key
```sh
node dist/index.js crete account2
```

Build and send transaction
```sh
node dist/index.js send account2 10 recipientPubKey -b
```