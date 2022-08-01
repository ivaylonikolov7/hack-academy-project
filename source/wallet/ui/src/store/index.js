import Vuex from "vuex";

import wallet from "./modules/wallet";
import accounts from "./modules/accounts";
import nodes from "./modules/nodes";

export default new Vuex.Store({
  modules: {
    wallet,
    accounts,
    nodes,
  },
});
