export interface CurrentUser {
    // none || volunteer || local_authority || provincial_admin
    // Todo: this will get much bigger when we have information from the client
    role: string;
}