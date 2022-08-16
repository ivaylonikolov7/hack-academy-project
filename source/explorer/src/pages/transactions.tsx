import {
  Box,
  Input,
  Button,
  FormControl,
  FormErrorMessage,
} from "@chakra-ui/react";
import axios from "axios";
import { useState } from "react";
import { Transactions } from "../components/Transactions";
import { Layout } from "../layout/Layout";
import { formatAddress } from "../utils/utils";

export const TransactionsPage = () => {
  const [transaction, setTransaction] = useState("");
  const [recipient, setRecipient] = useState("");
  const [hash, setHash] = useState("");
  const [value, setValue] = useState("");
  const [sender, setSender] = useState("");
  const [error, setError] = useState(false);

  return (
    <Layout background="#fff">
      <Box display="flex" padding="20px 20px">
        <FormControl isInvalid={error}>
          <Input
            placeholder="Type your tx here"
            onChange={(e) => {
              setTransaction(e.target.value);
            }}
          ></Input>
          <Button
            marginTop="20px"
            onClick={async () => {
              let response = await axios.get(
                "http://hackchain.pirin.pro/api/transactions/" + transaction
              );
              if (!response.data.data) {
                setError(true);
              }
              setHash(response.data.data.hash);
              setRecipient(response.data.data.recipient);
              setValue(response.data.data.value);
              setSender(response.data.data.sender);
            }}
          >
            Send
          </Button>
          <FormErrorMessage>No such tx.</FormErrorMessage>
        </FormControl>
      </Box>

      <Transactions
        txs={[
          {
            sender: formatAddress(sender),
            recipient: formatAddress(recipient),
            value,
            hash: formatAddress(hash),
          },
        ]}
      ></Transactions>
    </Layout>
  );
};
