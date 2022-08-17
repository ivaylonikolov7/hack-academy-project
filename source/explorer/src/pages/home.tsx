import axios from "axios";
import { useEffect, useState } from "react";
import BlockTable from "../components/BlockTable";
import { Layout } from "../layout/Layout";

export const Home = () => {
  const [txs, setTxs] = useState<
    [
      {
        index: number;
        txHeight: number;
        prevHash: string;
        difficulty: number;
        currentHash: string;
      }
    ]
  >([
    {
      index: 0,
      txHeight: 0,
      prevHash: "string",
      difficulty: 0,
      currentHash: "0",
    },
  ]);
  useEffect(() => {
    async function getLast10Blocks() {
      const response = await axios.get(
        "http://hackchain.pirin.pro/api/blocks/getlast/10"
      );
      const txsOutput = response.data.data.map((txOutput: any) => {
        return {
          currentHash: txOutput.currentBlockHash,
          index: txOutput.index,
          txHeight: txOutput.data.length,
          difficulty: txOutput.difficulty,
          prevHash: txOutput.previousBlockHash,
        };
      });
      setTxs(txsOutput);
    }
    getLast10Blocks();
  }, []);

  return (
    <Layout>
      <BlockTable txs={txs}></BlockTable>
    </Layout>
  );
};
