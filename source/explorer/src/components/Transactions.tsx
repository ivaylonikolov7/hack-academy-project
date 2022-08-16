import {
  Table,
  TableCaption,
  Tr,
  Th,
  Td,
  Text,
  Tbody,
  Thead,
} from "@chakra-ui/react";
import { formatAddress } from "../utils/utils";

export const Transactions = (props: { txs: any[] }) => {
  return (
    <>
      <Table>
        <TableCaption>Tx Logs</TableCaption>
        <Thead>
          <Tr>
            <Th>
              <Text>Hash</Text>
            </Th>
            <Th>
              <Text>Sender</Text>
            </Th>
            <Th>
              <Text>Recipient</Text>
            </Th>
            <Th>
              <Text>Value</Text>
            </Th>
            <Th>
              <Text>Fee</Text>
            </Th>
          </Tr>
        </Thead>
        <Tbody>
          {props.txs.map((tx) => {
            return (
              <Tr key={tx.hash}>
                <Td>{formatAddress(tx.hash)}</Td>
                <Td>{formatAddress(tx.sender)}</Td>
                <Td>{formatAddress(tx.recipient)}</Td>
                <Td>{tx.value}</Td>
                <Td>{tx.fee}</Td>
              </Tr>
            );
          })}
        </Tbody>
      </Table>
    </>
  );
};
