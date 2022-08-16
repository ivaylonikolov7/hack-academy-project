import { Container, Link, Box } from "@chakra-ui/react";
import { Link as ReachLink } from "react-router-dom";

export const Layout = function (props: any) {
  return (
    <>
      <Container background="#fff" maxWidth={"1000px"}>
        <Box display="flex" gap="10px" justifyContent="center">
          <Link as={ReachLink} to="/">
            Home
          </Link>
          <Link as={ReachLink} to="/account">
            Account
          </Link>
          <Link as={ReachLink} to="/block">
            Block
          </Link>
          <Link as={ReachLink} to="/transaction">
            Transaction
          </Link>
        </Box>
        <hr
          style={{
            marginBottom: "15px",
            marginTop: "15px",
          }}
        />
        {props.children}
      </Container>
    </>
  );
};
