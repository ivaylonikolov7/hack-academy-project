import {
  Box,
  Input,
  Button,
  FormControl,
  FormErrorMessage,
} from "@chakra-ui/react";
import axios from "axios";
import { useState } from "react";
import BlockTable from "../components/BlockTable";
import { Transactions } from "../components/Transactions";
import { Layout } from "../layout/Layout";

export const Block = () => {
  const [txHeight, setTxHeight] = useState(0);
  const [index, setIndex] = useState(0);
  const [currentHash, setCurrentHash] = useState("");
  const [prevHash, setPrevHash] = useState("");
  const [difficulty, setDifficulty] = useState(0);
  const [blockId, setBlockId] = useState(0);
  const [txs, setTxs] = useState<any[]>([]);
  const [error, setError] = useState(false);

  return (
    <Layout>
      <Box display="flex" padding="20px 20px">
        <FormControl isInvalid={error}>
          <Input
            placeholder="Type your block id here"
            onChange={(e) => {
              setBlockId(parseInt(e.target.value));
            }}
          ></Input>
          <Button
            marginTop="20px"
            onClick={async () => {
              const response = await axios.get(
                "http://hackchain.pirin.pro/api/blocks/" + blockId
              );
              if (!response.data.data) {
                setError(true);
                return;
              } else {
                setError(false);
              }
              setDifficulty(response.data.data.difficulty);
              setPrevHash(response.data.data.previousBlockHash);
              setCurrentHash(response.data.data.currentBlockHash);
              setIndex(response.data.data.index);
              setTxHeight(response.data.data.data.length);
              setTxs(response.data.data.data);
            }}
          >
            Send
          </Button>
          <FormErrorMessage>No such block .</FormErrorMessage>
        </FormControl>
      </Box>

      {!error && (
        <>
          <BlockTable
            txs={[
              {
                difficulty,
                currentHash,
                prevHash,
                index,
                txHeight,
              },
            ]}
          ></BlockTable>

          <Transactions txs={txs}></Transactions>
        </>
      )}
    </Layout>
  );
};
