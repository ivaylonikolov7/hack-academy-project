import { program } from 'commander';

import pkg from './package.json';
import Commands from './commands';

program.version(pkg.version, '-v', '--version');

program
    .command('create [alias]')
    .description('Genereate new keypair')
    .action(async (alias) => {
        console.log('generate new account with alias', alias);
        await Commands.createAccount(alias);
    });

program
    .command('import [alias]')
    .description('Import existing wallet')
    .option('--mnemonic <mnemonic>', 'Mnemonic')
    .option('--privateKey <privateKey>', 'Private key')
    .action(async (alias, options) => {
        await Commands.importAccount(alias, options.privateKey);
    });

program
    .command('config [key] [value]')
    .description('Wallet configurations')
    .action((key, value) => {
        if (key && !value) {
            console.log('get config key');
        } else if (key && value) {
            console.log('set config key');
        }
    });

program
    .command('balance [alias]')
    .description('Get balance if account')
    .action((alias) => {
        console.log('get balance of account ', alias);
    });

program
    .command('send [alias] [amount] [recipient]')
    .description('Send transaction')
    .option('-b', '--broadcast', 'Broadcast tx to node')
    .option('-s', '--sign', 'Sign tx')
    .option('-o', '--output <file>', 'Output tx if not directly broadcasted')
    .option('-i', '--input <file>', 'Path to local tx file')
    .action(async (alias, amount, recipient, options) => {
        await Commands.createTransaction(alias, amount, recipient, options.b === true);
    })


program.parse(process.argv);


if (!program.args.length) {
    program.help();
}