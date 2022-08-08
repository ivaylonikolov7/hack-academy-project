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
    init({ commit, dispatch, rootGetters }, { accounts = [], nodes = [] } = {}) {
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

      const wallet = new Wallet({
        nodes,
      });

      // TODO: get cached selected node

      const selectedNode = mainnet.id;

      wallet.selectNode(selectedNode);

      accounts.forEach((account, idx) =>
        wallet.addAccount(account, { selected: idx === 0 })
      );

      commit("setInstance", wallet);

      dispatch("accounts/fetchBalances", {}, { root: true });
      dispatch("accounts/fetchTransactions", {}, { root: true });

      setInterval(() => {
        dispatch("accounts/fetchBalances", {}, { root: true });
        dispatch("accounts/fetchTransactions", {}, { root: true });
      }, 10000);
    },

    create({ dispatch, commit }) {
      const account = new Account();

      commit("accounts/add", account, { root: true });

      dispatch("init", { accounts: [account] });
    },

    import({ dispatch, commit }, privateKey) {
      const account = new Account(privateKey);

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
