import axios from "axios";
import { useEffect } from "react";

export const Home = () => {
  useEffect(() => {
    async function getLast10Blocks() {
      let response = await axios.get(
        "http://hackchain.pirin.pro/api/blocks/getlast/10"
      );
      console.log(response);
    }

    getLast10Blocks();
  });

  return <></>;
};
