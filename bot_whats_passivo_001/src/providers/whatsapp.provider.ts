import { create, Message, Whatsapp } from 'venom-bot';
import { ConversaChatBot } from '../model/conversaChatBot';
import moment from 'moment';
import { environment } from '../config/environment';
import { getLogger } from '../logConfig';
import { Logger, ILogObj } from "tslog";

export class WhatsAppProvider {
    private sessionName: string;
    private URL = environment.baseUrl;
    private token: string;
    private timers: Map<string, NodeJS.Timeout> = new Map();
    
    private logModel = getLogger("model");
    private log: Logger<ILogObj> = new Logger({
      prettyLogTemplate: "{{yyyy}}.{{mm}}.{{dd}} {{hh}}:{{MM}}:{{ss}}:{{ms}}\t{{logLevelName}}\t",
    });


    constructor(
      sessionName: string
      ) {
      this.sessionName = sessionName;
    }

    async init() {
        create({
            session: this.sessionName,
            autoClose: 0,
	    headless: 'new'
        }).then((client: Whatsapp) => {
          this.log.debug("Init Robo..");
          this.start(client);
        }).catch((erro) => {
            this.log.error("Create erro.", erro);
        });
    }

    start(client: Whatsapp) {
  client.onMessage(async (message: Message) => {
    await this.login();

    const from = message.from;
    this.log.debug('message', message);

    this.resetUserTimer(client, from);

    try {
      const response = await fetch(
        `${this.URL}InteracaoChat?telefone=${from.substring(2, from.length - 5)}&mensagem=${encodeURIComponent(message.body)}`,
        {
          method: 'POST',
          headers: { Authorization: 'Bearer ' + this.token },
        }
      );

      if (!response.ok) {
        this.log.error('Erro ConversaChatBot, status:', response.status);
        return;
      }

      const data: any = await response.json();
      this.log.debug('API response', data);

      const midia: number = typeof data.midia !== 'undefined' ? data.midia : data.tipo;

      if (midia === 1) {
        await client.sendText(
          message.from,
          data.mensagem.replace(/\\n/g, "\n")
        );
        this.log.info('Enviado (texto) para', from);
      } else if (midia === 2) {
        await client.sendImage(
          message.from,
          data.url,
          data.imagemName ?? 'image',
          data.mensagem.replace(/\\n/g, "\n")
        );
        this.log.info('Enviado (imagem) para', from);
      } else if (midia === 3) {
        await client.sendFile(
          message.from,
          data.url,
          data.imagemName ?? 'video.mp4',
          data.mensagem.replace(/\\n/g, "\n")
        );
        this.log.info('Enviado (video) para', from);
      } else {
        this.log.error('Resposta inválida: midia', midia, 'ou falta url');
      }

      if (data.finalizar) {
        this.clearTimer(from);
      }
    } catch (erro) {
      this.log.error('Erro ao processar mensagem:', erro);
    }
  });

  process.on('SIGINT', () => this.clearAllTimers());
  process.on('SIGTERM', () => this.clearAllTimers());
}

    private resetUserTimer(client: Whatsapp, from: string) {
        this.clearTimer(from);
        const timer = global.setTimeout(async () => {
            try {
                await this.resetConversation(from);
                await client.sendText(from, 'conversation timed out');
            } catch (erro) {
                this.log.error('Erro timeout handler:', erro);
            } finally {
                this.clearTimer(from);
            }
        }, 5 * 60 * 1000);
        this.timers.set(from, timer);
    }

    private clearTimer(from: string) {
        const timer = this.timers.get(from);
        if (timer) {
            clearTimeout(timer);
            this.timers.delete(from);
        }
    }

    private clearAllTimers() {
        for (const [, timer] of this.timers) {
            clearTimeout(timer);
        }
        this.timers.clear();
    }

    private async resetConversation(from: string) {
        try {
            await fetch(`${this.URL}ResetConversa?telefone=${from.substring(2, from.length - 5)}`, {
                method: 'POST',
                headers: { Authorization: 'Bearer ' + this.token },
            });
        } catch (erro) {
            this.log.error('Erro resetConversation:', erro);
        }
    }


    async login(){
	this.log.debug("login IN:");
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

      const content = await response.json();

      this.token = content.token;
      return content;
    }
}