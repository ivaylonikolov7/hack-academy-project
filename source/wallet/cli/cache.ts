import findCacheDir from "find-cache-dir";
import fs from "fs-extra";


const cacheDir = findCacheDir({ name: 'hackchain-wallet-cli' });


export default {
    async getKey(k: string) {
        try {
            const data = await fs.readFile(`${cacheDir}/${k}.json`, "utf8");

            return data;
        } catch (e) {
            return false;
        }
        
    },

    async setKey(k: string, v: any, json?: boolean) {

        try {
            await fs.outputFile(`${cacheDir}/${k}.json`, json ? JSON.stringify(v) : v);
        } catch (e) {
            return false;
        }
    },

    async getOrSetKey(k: string, c: Function) {
        const data = this.getKey(k);

        if (!data) {

        }
    }
};