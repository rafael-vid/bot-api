export interface Alerta {
    Id: string,
    canal: string,
    acao: string, 
    mensagem: string, 
    tipo: string, 
    dataenvio?: Date 
    dataconfirmacao?: Date,
    nome: string, 
    email: string, 
    telefone: string
}