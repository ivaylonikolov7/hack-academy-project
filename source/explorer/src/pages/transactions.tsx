import { Container, Box, Input, Button } from "@chakra-ui/react";
import axios from "axios";
import { useState } from "react";
import { Transactions } from "../components/Transactions";
import { formatAddress } from "../utils/utils";

export const TransactionsPage = () => {
  const [transaction, setTransaction] = useState("");
  const [recipient, setRecipient] = useState("");
  const [hash, setHash] = useState("");
  const [value, setValue] = useState("");
  const [sender, setSender] = useState("");

  return (
    <Container background="#fff">
      <Box display="flex" padding="20px 20px">
        <Input
          placeholder="Type your tx here"
          onChange={(e) => {
            setTransaction(e.target.value);
          }}
        ></Input>
        <Button
          marginLeft={"2"}
          onClick={async () => {
            let response = await axios.get(
              "http://hackchain.pirin.pro/api/transactions/" + transaction
            );
            setHash(response.data.data.hash);
            setRecipient(response.data.data.recipient);
            setValue(response.data.data.value);
            setSender(response.data.data.sender);
          }}
        >
          Send
        </Button>
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
    </Container>
  );
};
