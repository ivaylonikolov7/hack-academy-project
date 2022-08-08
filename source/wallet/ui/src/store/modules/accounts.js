export default {
  namespaced: true,
  state: {
    list: [],
    active: 0,
    balance: 0,
    transactions: [],
  },
  mutations: {
    add(state, account) {
      state.list.push(account);
      localStorage.setItem(
        "accounts",
        JSON.stringify(
          state.list.map((a) => ({
            publicKey: a.publicKey,
            privateKey: a.privateKey,
          }))
        )
      );
    },
    setBalance(state, balance) {
      state.balance = balance;
    },
    setTransactions(state, transactions) {
      state.transactions = transactions;
    },
  },
  getters: {
    active(state) {
      return state.list[state.active];
    },
  },
  actions: {
    async fetchBalances({ commit, rootState }) {
      const accountInfo =
        await rootState.wallet.instance.getActiveAccountInfo();

      commit("setBalance", accountInfo.balance);
    },

    async fetchTransactions({ commit, rootState }) {
      const txs = await rootState.wallet.instance.getAccountTxs();

      commit("setTransactions", txs);
    },
  },
};
