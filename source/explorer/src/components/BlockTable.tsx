import {
  Table,
  TableCaption,
  Tr,
  Th,
  Td,
  Text,
  Thead,
  Tbody,
} from "@chakra-ui/react";
import { formatAddress } from "../utils/utils";

export default function BlockTable(props: {
  txs: [
    {
      index: number;
      txHeight: number;
      prevHash: string;
      difficulty: number;
      currentHash: string;
    }
  ];
}) {
  return (
    <>
      <Table>
        <TableCaption>Block Logs</TableCaption>
        <Thead>
          <Tr>
            <Th>
              <Text>Index</Text>
            </Th>
            <Th>
              <Text>Tx Height</Text>
            </Th>
            <Th>
              <Text>Current block hash</Text>
            </Th>
            <Th>
              <Text>Difficulty</Text>
            </Th>
            <Th>
              <Text>Previous block hash</Text>
            </Th>
          </Tr>
        </Thead>
        <Tbody>
          {props.txs.map((block) => (
            <Tr key={block.currentHash}>
              <Td>{block.index}</Td>
              <Td>{block.txHeight}</Td>
              <Td>{formatAddress(block.currentHash)}</Td>
              <Td>{block.difficulty}</Td>
              <Td>{formatAddress(block.prevHash)}</Td>
            </Tr>
          ))}
        </Tbody>
      </Table>
    </>
  );
}
