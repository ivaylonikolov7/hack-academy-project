export default {
  namespaced: true,
  state: {
    list: {
      mainnet: {
        id: "mainnet",
        name: "Mainnet",
        url: "http://hackchain.pirin.pro",
      },
    },
    active: "mainnet",
  },
  mutations: {
    set(state, nodes) {
      console.log(nodes);
    },
  },
  getters: {
    active(state) {
      return state.list[state.active];
    },
  },
  actions: {
    
  },
};
