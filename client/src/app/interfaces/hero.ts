import { Power } from './power';

export interface Hero {
  id: string;
  name: string;
  power: Power;
  image?: string;
  alterEgo?: string;
}

export interface NewHeroForm {
  name: string;
  power: Power;
  image?: string;
  alterEgo?: string;
}
