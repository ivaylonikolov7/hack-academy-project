import Vuex from "vuex";

import accountsModule from "./modules/accounts";

export default new Vuex.Store({
  modules: {
    accounts: accountsModule,
  },
});
