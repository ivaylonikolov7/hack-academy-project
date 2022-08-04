export const hexToBase64 = (hexStr: string): string => {
    return btoa([...hexStr].reduce((acc, _, i) =>
        acc += !(i - 1 & 1) ? String.fromCharCode(parseInt(hexStr.substring(i - 1, i + 1), 16)) : "" 
    ,""));
}