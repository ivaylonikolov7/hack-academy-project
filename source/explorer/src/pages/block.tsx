import { Container, Box, Input, Button } from "@chakra-ui/react";
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

  return (
    <Layout>
      <Box display="flex" padding="20px 20px">
        <Input
          placeholder="Type your block id here"
          onChange={(e) => {
            setBlockId(parseInt(e.target.value));
          }}
        ></Input>
        <Button
          marginLeft={"2"}
          onClick={async () => {
            let response = await axios.get(
              "http://hackchain.pirin.pro/api/blocks/" + blockId
            );
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
      </Box>

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
    </Layout>
  );
};
