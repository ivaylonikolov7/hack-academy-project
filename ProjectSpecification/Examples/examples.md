# Examples
## Keys
```
User1
Private key (decimal): '59693103339044192723222300130357326931590556608121457583336725385068986603189'
Private key (hex): '83f919649688da47e81ea3802d49b902d0367b027ca708dbf7a078b844f196b5'
Public key (x,y): '(140188cbfa31e9364dcfe0b6204b4ff7913daf66da8cf59eaf4694ed5d3774e4,24b20a1a95a829976778ac280496dc4cdd1d1fdfab649cba799d084dcf0cc296,1,0)'
Public key (hex): '04140188cbfa31e9364dcfe0b6204b4ff7913daf66da8cf59eaf4694ed5d3774e424b20a1a95a829976778ac280496dc4cdd1d1fdfab649cba799d084dcf0cc296'

User2
Private key (decimal): '75371236531580909925074358309683361378525293362227187819130742507447782904113'
Private key (hex): 'a6a29bad474ff512797e15b6fe0e1ae9b1044c247694119accf7f9f009d69131'
Public key (x,y): '(4842ce6522e4442ccf446c9d28e7be0aa26b83934d60289e3c1f9eba49e44dd6,134b7b22ff8a38e86e69683843e3f058f326001ff4fee56e94a1e9681cda4bda,1,0)'
Public key (hex): '044842ce6522e4442ccf446c9d28e7be0aa26b83934d60289e3c1f9eba49e44dd6134b7b22ff8a38e86e69683843e3f058f326001ff4fee56e94a1e9681cda4bda'
```
## Transaction
```
Transaction raw:
'{
  "Sender": "04140188cbfa31e9364dcfe0b6204b4ff7913daf66da8cf59eaf4694ed5d3774e424b20a1a95a829976778ac280496dc4cdd1d1fdfab649cba799d084dcf0cc296",
  "Recipient": "044842ce6522e4442ccf446c9d28e7be0aa26b83934d60289e3c1f9eba49e44dd6134b7b22ff8a38e86e69683843e3f058f326001ff4fee56e94a1e9681cda4bda",
  "Nonce": 1,
  "Value": 1000,
  "Fee": 5
}'

Transaction for hashing:
'{"Sender":"04140188cbfa31e9364dcfe0b6204b4ff7913daf66da8cf59eaf4694ed5d3774e424b20a1a95a829976778ac280496dc4cdd1d1fdfab649cba799d084dcf0cc296","Recipient":"044842ce6522e4442ccf446c9d28e7be0aa26b83934d60289e3c1f9eba49e44dd6134b7b22ff8a38e86e69683843e3f058f326001ff4fee56e94a1e9681cda4bda","Nonce":1,"Value":1000,"Fee":5}'

Transaction hash:
'00e7db18e626479cca9cd9d170eaef07f50a0c27d7d090c60da0ff7ef9b7d088'

Transaction signature:
'MEQCICrgao5NeTMp4kpXKHLtYpW0fFGD/im3THAp5H/YhsHKAiAVP/40L/ybdjdHUQbCYhUV18KLJa+JVnO3Hc4e+kWe7w=='

Transaction full:
'{
  "Sender": "04140188cbfa31e9364dcfe0b6204b4ff7913daf66da8cf59eaf4694ed5d3774e424b20a1a95a829976778ac280496dc4cdd1d1fdfab649cba799d084dcf0cc296",
  "Recipient": "044842ce6522e4442ccf446c9d28e7be0aa26b83934d60289e3c1f9eba49e44dd6134b7b22ff8a38e86e69683843e3f058f326001ff4fee56e94a1e9681cda4bda",
  "Nonce": 1,
  "Value": 1000,
  "Fee": 5,
  "Hash": "00e7db18e626479cca9cd9d170eaef07f50a0c27d7d090c60da0ff7ef9b7d088",
  "Signature": "MEQCICrgao5NeTMp4kpXKHLtYpW0fFGD/im3THAp5H/YhsHKAiAVP/40L/ybdjdHUQbCYhUV18KLJa+JVnO3Hc4e+kWe7w=="
}'

Transaction isValid:
'True'
```
## Block
```
Block raw:
'{
  "Index": 1,
  "Timestamp": 1658892871,
  "Data": [
    {
      "Sender": "04140188cbfa31e9364dcfe0b6204b4ff7913daf66da8cf59eaf4694ed5d3774e424b20a1a95a829976778ac280496dc4cdd1d1fdfab649cba799d084dcf0cc296",
      "Recipient": "044842ce6522e4442ccf446c9d28e7be0aa26b83934d60289e3c1f9eba49e44dd6134b7b22ff8a38e86e69683843e3f058f326001ff4fee56e94a1e9681cda4bda",
      "Nonce": 1,
      "Value": 1000,
      "Fee": 5
    }
  ],
  "PreviousBlockHash": "none",
  "Nonce": 0,
  "Difficulty": 5
}'

Block for hashing:
'{"Index": 1,"Timestamp": 1658892871,"Data": [{"Sender":"04140188cbfa31e9364dcfe0b6204b4ff7913daf66da8cf59eaf4694ed5d3774e424b20a1a95a829976778ac280496dc4cdd1d1fdfab649cba799d084dcf0cc296","Recipient":"044842ce6522e4442ccf446c9d28e7be0aa26b83934d60289e3c1f9eba49e44dd6134b7b22ff8a38e86e69683843e3f058f326001ff4fee56e94a1e9681cda4bda","Nonce":1,"Value":1000,"Fee":5}]],"PreviousBlockHash": "none","Nonce": 0,"Difficulty": 5}'

Block hash:
''

Block full:
'{
  "Index": 1,
  "Timestamp": 1658892871,
  "Data": [
    {
      "Sender": "04140188cbfa31e9364dcfe0b6204b4ff7913daf66da8cf59eaf4694ed5d3774e424b20a1a95a829976778ac280496dc4cdd1d1fdfab649cba799d084dcf0cc296",
      "Recipient": "044842ce6522e4442ccf446c9d28e7be0aa26b83934d60289e3c1f9eba49e44dd6134b7b22ff8a38e86e69683843e3f058f326001ff4fee56e94a1e9681cda4bda",
      "Nonce": 1,
      "Value": 1000,
      "Fee": 5
    }
  ],
  "PreviousBlockHash": "none",
  "Nonce": 0,
  "Difficulty": 5,
  "CurrentBlockHash": ""
}'
```