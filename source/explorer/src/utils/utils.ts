export function formatAddress(account: string): string {
  if (account.length === 0) {
    return "none";
  }
  if (account.length < 8) {
    return account;
  }
  return (
    account.slice(0, 5) +
    "..." +
    account.slice(account.length - 5, account.length)
  );
}
