import elliptic from 'elliptic';

export const hexToBase64 = (hexStr: string): string => {
    if (typeof btoa === "undefined") {
        return Buffer.from(hexStr, "hex").toString("base64");
    }

    return btoa([...hexStr].reduce((acc, _, i) =>
        acc += !(i - 1 & 1) ? String.fromCharCode(parseInt(hexStr.substring(i - 1, i + 1), 16)) : "" 
    ,""));
}

export const validPublicKey = (publicKey: string): boolean => {
    try {
        const ec = new elliptic.ec('secp256k1');

        ec.keyFromPublic(publicKey, 'hex');

        return true;
    } catch (e) {
        return false;
    }
}