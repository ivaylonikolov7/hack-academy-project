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
    select(state, idx) {
      state.active = idx;
    },
  },
  getters: {
    active(state) {
      return state.list[state.active];
    },
    list(state) {
      return state.list.map((a) => a.publicKey);
    },
  },
  actions: {
    async fetchBalances({ commit, rootState }) {
      const accountInfo =
        await rootState.wallet.instance.getActiveAccountInfo();

      commit("setBalance", accountInfo?.balance ?? 0);
    },

    async fetchTransactions({ commit, rootState }) {
      const txs = await rootState.wallet.instance.getAccountTxs();

      if (txs) {
        commit("setTransactions", txs);
      }
    },
  },
};
