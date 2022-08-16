import { useState } from "react";
import {
  Input,
  Box,
  Button,
  FormControl,
  FormErrorMessage,
} from "@chakra-ui/react";
import { AccountTable } from "../components/AccountTable";
import axios from "axios";
import { Transactions } from "../components/Transactions";
import { Layout } from "../layout/Layout";

export const Account = () => {
  const [inputAccount, setInputAccount] = useState("0");
  const [account, setAccount] = useState("0");
  const [balance, setBalance] = useState(0);
  const [nonce, setNonce] = useState(0);
  const [txs, setTxs] = useState<any[]>([]);
  const [error, setError] = useState(false);

  return (
    <>
      <Layout>
        <Box padding="20px 20px">
          <FormControl isInvalid={error}>
            <Input
              placeholder="Type your account here"
              onChange={(e) => {
                setInputAccount(e.target.value);
              }}
            ></Input>
            <Button
              marginTop="20px"
              onClick={async () => {
                let account = await axios.get(
                  "http://hackchain.pirin.pro/api/accounts/" + inputAccount
                );

                console.log(account);

                if (!account.data.data) {
                  setError(true);
                  return;
                } else {
                  setError(false);
                }
                setBalance(account.data.data.balance);
                setNonce(account.data.data.nonce);
                setAccount(account.data.data.address);

                let accountTxs = await axios.get(
                  "http://hackchain.pirin.pro/api/accounts/" +
                    inputAccount +
                    "/transactions"
                );
                setTxs(accountTxs.data.data);
              }}
            >
              Send
            </Button>
            <FormErrorMessage>No such account .</FormErrorMessage>
          </FormControl>
        </Box>
        {!error && (
          <>
            <AccountTable
              account={account}
              balance={balance}
              nonce={nonce}
            ></AccountTable>

            <Transactions txs={txs}></Transactions>
          </>
        )}
      </Layout>
    </>
  );
};
