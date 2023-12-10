import { Component, OnInit, inject } from '@angular/core';
import { NewHeroForm } from '../../interfaces/hero';
import { JsonPipe, NgFor, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HeroService } from '../../services/hero.service';
import { Power } from '../../interfaces/power';
import { HeroPowerService } from '../../services/hero-power.service';
import { BlobService } from '../../services/blob.service';

interface FormModel {
  name: string;
  power: Power;
  image: File | null | undefined;
  alterEgo: string;
  powerIndex: number;
}

@Component({
  selector: 'app-new-hero',
  standalone: true,
  imports: [NgFor, FormsModule, JsonPipe, NgIf],
  templateUrl: './new-hero.component.html',
  styleUrl: './new-hero.component.scss',
  host: {
    ngSkipHydration: 'true',
  },
})
export class NewHeroComponent implements OnInit {
  powers: Power[] = [];
  heroService = inject(HeroService);
  heroPowerService = inject(HeroPowerService);
  blobService = inject(BlobService);

  model: FormModel = {
    image: null,
    name: 'Dr. IQ',
    powerIndex: 0,
    power: this.powers[0],
    alterEgo: 'Chuck Overstreet',
  };

  submitted = false;

  ngOnInit(): void {
    this.heroPowerService
      .getPowers()
      .subscribe((powers) => powers && (this.powers = powers));
  }

  getImagePreview(): string | null {
    return this.model.image ? URL.createObjectURL(this.model.image) : null;
  }

  async onSubmit() {
    let heroImage: string | null = null;
    const inputImage = this.model.image;

    if (inputImage) {
      heroImage = await this.blobService.upsertHeroImage(
        this.model.name,
        inputImage
      );
    }

    const newHero: NewHeroForm = {
      name: this.model.name,
      power: this.powers[this.model.powerIndex],
      alterEgo: this.model.alterEgo,
      image: heroImage ? heroImage : undefined,
    };
    await this.heroService.addHero(newHero);
    this.submitted = true;
  }
}
