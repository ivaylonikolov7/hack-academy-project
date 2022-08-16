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

export const AccountTable = (props: {
  balance: number;
  nonce: number;
  account: string;
}) => {
  return (
    <>
      <Table>
        <TableCaption>Аccount Logs</TableCaption>
        <Thead>
          <Tr>
            <Th>
              <Text>Address</Text>
            </Th>
            <Th>
              <Text>Value</Text>
            </Th>
            <Th>
              <Text>Nonce</Text>
            </Th>
          </Tr>
        </Thead>
        <Tbody>
          <Tr>
            <Td>{formatAddress(props.account)}</Td>
            <Td>{props.balance}</Td>
            <Td>{props.nonce}</Td>
          </Tr>
        </Tbody>
      </Table>
    </>
  );
};
