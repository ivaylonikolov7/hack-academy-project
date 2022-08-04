export default {
  namespaced: true,
  state: {
    list: [],
    active: 0,
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
  },
  getters: {
    active(state) {
      return state.list[state.active];
    },
  },
};
