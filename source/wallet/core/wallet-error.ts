class WalletError extends Error {
    public error;

    constructor(error: any) {
        super(error.error);
        this.error = error;

        Object.setPrototypeOf(this, WalletError.prototype);
    }

    getErrorMessage(): string {
        return this.error.error;
    }
}

export default WalletError;
