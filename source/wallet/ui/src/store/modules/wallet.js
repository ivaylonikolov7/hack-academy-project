import Node from "../../../../core/node";
import Account from "../../../../core/account";
import Wallet from "../../../../core/wallet";

export default {
  namespaced: true,
  state: {
    instance: null,
  },
  mutations: {
    setInstance(state, instance) {
      state.instance = instance;
    },
  },
  actions: {
    init({ commit, rootGetters }, { accounts = [], nodes = [] } = {}) {
      console.log('tuk11');
      const mainnet = rootGetters["nodes/active"];
      if (!nodes.length) {
        const node = new Node(mainnet.id, mainnet.url);

        console.log(node instanceof Account);

        nodes.push(node);
      }

      if (accounts.length) {
        accounts = accounts.map((a) => {
          if (!(a instanceof Account)) {
            a = new Account(a.privateKey);
          }

          commit("accounts/add", a, { root: true });

          return a;
        });
      }
      // const account = new Account(
      //   "83f919649688da47e81ea3802d49b902d0367b027ca708dbf7a078b844f196b5"
      // );

      // console.log(accounts);

      // console.log("nodes", nodes);

      const wallet = new Wallet({
        nodes,
      });

      // TODO: get cached selected node

      const selectedNode = mainnet.id;

      wallet.selectNode(selectedNode);

      accounts.forEach((account, idx) =>
        wallet.addAccount(account, { selected: idx === 0 })
      );

      console.log(wallet);

      commit("setInstance", wallet);
    },

    create({ dispatch, commit }) {
      const account = new Account();

      commit("accounts/add", account, { root: true });

      dispatch("init", { accounts: [account] });
    },

    restore({ dispatch }) {
      const accounts = window.localStorage.getItem("accounts")
        ? JSON.parse(window.localStorage.getItem("accounts"))
        : [];
      const nodes = window.localStorage.getItem("nodes")
        ? JSON.parse(window.localStorage.getItem("nodes"))
        : [];

      if (!accounts.length) {
        return false;
      }

      // console.log("accounts", accounts);

      console.log('tuk');

      return dispatch("init", { accounts, nodes });
    },
  },
};
