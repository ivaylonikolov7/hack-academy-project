import Node from "hackchain-wallet-core/node";
import Account from "hackchain-wallet-core/account";
import Wallet from "hackchain-wallet-core/wallet";

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
    init(
      { commit, dispatch, rootGetters },
      { accounts = [], nodes = [] } = {}
    ) {
      const mainnet = rootGetters["nodes/active"];
      if (!nodes.length) {
        const node = new Node(mainnet.id, mainnet.url);

        nodes.push(node);
      }

      if (accounts.length) {
        accounts = accounts.map((a) => {
          if (!(a instanceof Account)) {
            a = new Account(a.privateKey);
            commit("accounts/add", a, { root: true });
          }

          return a;
        });
      }

      const wallet = new Wallet({
        nodes,
      });

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

    create({ dispatch, commit, state }) {
      const account = new Account();
      commit("accounts/add", account, { root: true });
      dispatch("init", { accounts: [account] });
      state.instance.selectAccount(account.publicKey);
      commit("accounts/selectLatest", {}, { root: true });
    },

    import({ state, dispatch, commit }, privateKey) {
      const account = new Account(privateKey);
      commit("accounts/add", account, { root: true });
      dispatch("init", { accounts: [account] });
      state.instance.selectAccount(account.publicKey);
      commit("accounts/selectLatest", {}, { root: true });
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

      return dispatch("init", { accounts, nodes });
    },

    selectAccount({ state, commit, dispatch }, { address, idx }) {
      state.instance.selectAccount(address);
      commit("accounts/select", idx, { root: true });
      dispatch("accounts/fetchBalances", {}, { root: true });
      dispatch("accounts/fetchTransactions", {}, { root: true });
    },

    exportPrivKey({ rootGetters }) {
      alert(
        `Your private key is:  ${rootGetters["accounts/active"].privateKey}`
      );
    },
  },
};
