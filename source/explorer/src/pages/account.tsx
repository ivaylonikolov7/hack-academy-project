import { useState } from "react";
import { Container, Input, Box, Button } from "@chakra-ui/react";
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

  return (
    <>
      <Layout>
        <Box display="flex" padding="20px 20px">
          <Input
            placeholder="Type your account here"
            onChange={(e) => {
              setInputAccount(e.target.value);
            }}
          ></Input>
          <Button
            marginLeft={"2"}
            onClick={async () => {
              let account = await axios.get(
                "http://hackchain.pirin.pro/api/accounts/" + inputAccount
              );
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
        </Box>
        <AccountTable
          account={account}
          balance={balance}
          nonce={nonce}
        ></AccountTable>

        <Transactions txs={txs}></Transactions>
      </Layout>
    </>
  );
};
