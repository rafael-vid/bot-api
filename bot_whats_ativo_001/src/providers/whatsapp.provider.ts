import { create, Message, Whatsapp } from 'venom-bot';
import { ConversaChatBot } from '../model/conversaChatBot';
import moment from 'moment';
import { environment } from '../config/environment';
import { setTimeout } from "timers/promises";
import { getLogger } from '../logConfig';
import { Logger, ILogObj } from "tslog";
import { Alerta } from '../model/alertas';

export class WhatsAppProvider {
    private sessionName: string;
    private URL = environment.baseUrl;
    private token: string;
    private client: Whatsapp

    private logModel = getLogger("model");
    private log: Logger<ILogObj> = new Logger({
        prettyLogTemplate: "{{yyyy}}.{{mm}}.{{dd}} {{hh}}:{{MM}}:{{ss}}:{{ms}}\t{{logLevelName}}\t",
    });


    constructor(
        sessionName: string
    ) {
        this.sessionName = sessionName;

        this.getFila = this.getFila.bind(this);
        this.sendMessageToWhatsapp = this.sendMessageToWhatsapp.bind(this);
        this.sendImage = this.sendImage.bind(this);
        this.marcaMensagemEnviada = this.marcaMensagemEnviada.bind(this);
    }

    async init() {
        create({
            session: this.sessionName,
            autoClose: 0,
            headless: 'new'
        }).then((client: Whatsapp) => {
            this.client = client;
            this.log.info("Init Robo.");
            this.listen();
        }).catch((erro) => {
            this.log.error("Create erro.", erro);
        });
    }

    async listen() {

        await this.login();

        //Consular Api 
        this.ConsultaUsuarioWhats();

        this.client.onMessage(async (message) => {

        await this.marcaMensagemRecebida(message.from, message.timestamp).then(r => {});
      });
    }

    async login(){
      const response = await fetch(this.URL + "Login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          "user": environment.userApi,
          "secret": environment.secretApi
        })
      });
  
      return response.json()
      .then(r => {
        this.token = r.token;
      }).catch((err) => {
        this.log.error("login erro.", err);
      });
    }

    async marcaMensagemEnviada(IdAlerta: string){
      this.log.info("---marcaMensagemEnviada---");
      try {
        const response = await fetch(this.URL + "MarcaMensagemEnviada?id=" + IdAlerta, {
          method: "PUT",
          headers: {
            Authorization: "Bearer " + this.token
          }
        });
        return response;
      } catch (error) {
        this.log.error("Erro marcaMensagemEnviada:", error);
        this.repetirConsulta();
      }
    }

    async sendMessageToWhatsapp(from: string, msg: string, IdAlerta: string) {
        this.log.info("---sendMessageToWhatsapp---");
        return new Promise((resolve, reject) => {
            this.client
                .sendText(from, msg)
                .then(async (result: any) => {
                    if (!result.erro) {
                        await this.marcaMensagemEnviada(IdAlerta).then(_ => {
                        }).catch(e => {
                            this.log.error("Erro marcaMensagemEnviada:", e);
                        })
                    }
                    resolve(result)
                })
                .catch(async (erro) => {
                    this.log.error('Error sendMessageToWhatsapp: ', erro);
                    await this.marcaMensagemEnviada(IdAlerta).then(_ => {
                    }).catch(e => {
                        this.log.error("Erro marcaMensagemEnviada:", e);
                    })
                    reject(erro)
                });
        });
    }

    async sendImage(from: string, url: string, filename: string, caption: string, IdAlerta: string) {
        this.log.info("---sendImage---");
        return new Promise((resolve, reject) => {
            this.client
                .sendImage(from, url, filename, caption)
                .then(async (result: any) => {
                    if (!result.erro) {
                        await this.marcaMensagemEnviada(IdAlerta).catch(e => {
                            this.log.error("Erro marcaMensagemEnviada:", e);
                        });
                    }
                    resolve(result);
                })
                .catch(async (erro) => {
                    this.log.error('Error sendImage: ', erro);
                    await this.marcaMensagemEnviada(IdAlerta).catch(e => {
                        this.log.error("Erro marcaMensagemEnviada:", e);
                    });
                    reject(erro);
                });
        });
    }

    async ConsultaUsuarioWhats() {
        this.log.info("---ConsultaUsuarioWhats---");
        await this.getFila().then(async r => {
            const result: Alerta[] = r;

            if (result) {
                for (const [index, response] of result.entries()) {
                    try {

                        if ((index % 20) == 0) {
                            await setTimeout(20000);
                        }
                        if (response.telefone.length == 13) {

                            const numberDDI = response.telefone.substring(0, 2);
                            const numberDDD = response.telefone.substring(2, 4);
                            const numberUser = response.telefone.substring(4);

                            const number = numberDDI + numberDDD /*+ ((parseInt(numberDDD) >= 30) ? "9" : "")*/ + numberUser + "@c.us";

                            this.log.info("Enviando... ", number);
                            if (Number(response.tipo) === 2 && response.url) {
                                await this.sendImage(number, response.url, response.imagemName ?? 'image', response.mensagem, response.Id);
                            } else {
                                await this.sendMessageToWhatsapp(number, response.mensagem, response.Id);
                            }
                            await setTimeout(36000);
                        }
                    } catch (error: any) {
                        this.log.error("Erro Enviar Mensagem: ", error.text);
                    }
                }
            }

            await setTimeout(30000);
            this.repetirConsulta();

        }).catch((err) => {
            this.log.error("Erro ConsultaUsuarioWhats: ", err);
            //this.repetirConsulta();
        })
    }

    async repetirConsulta() {
        this.log.info("---RepetirConsulta---");
        await this.login();
        this.ConsultaUsuarioWhats()
    }

    async getFila() {
        this.log.info("---getFila---")
        try {
            const getNumeros = await fetch(this.URL + "GetFila", {
                headers: {
                    Authorization: "Bearer " + this.token
                }
        });
        return getNumeros.json();
      } catch (error) {
        this.log.error("Erro getFila: ", error);
        this.repetirConsulta();
      }
    }
  
    async marcaMensagemRecebida(telefone: string, data:number){
      this.log.info("---marcaMensagemRecebida---");
      try {
        const response = await fetch(`${this.URL}MarcaMensagemRecebida?telefone=${telefone.replace("@c.us","")}&data=${moment.unix(data).format('YYYY-MM-DDThh:mm:ss[Z]')}`, {
          method: "PUT",
          headers: {
            Authorization: "Bearer " + this.token
          }
        });
        return response;
      } catch (error) {
        this.log.error("Erro marcaMensagemRecebida:", error);
        this.repetirConsulta();
      }
      
    }
}