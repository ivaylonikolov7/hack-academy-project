### Consensus

1. Transaction validation steps
    - check from/to addresses are valid - can be parsed to a hex number
    - check balance is enough - value should be positive integer
    - check fee - should be positive integer
    - check nonce is exactly the next one
    - check signature - should be valid for the from address(public key)
    - check hash - sha256
2. Block validation steps
    - Longest chain - seem like a simpler solution
    - block number to be the next one
    - previous block hash to match
    - timestamp to be AFTER the previous block's timestamp
    - all transactions to be valid
    - coinbase/reward transaction value should be block reward + fees and not more 