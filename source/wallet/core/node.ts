class Node {
    public id: string;
    public url: string;

    constructor(id: string, url: string) {
        this.id = id;
        this.url = url;
    }

    async sendTransaction(tx: Object) {

    }
}

export default Node;